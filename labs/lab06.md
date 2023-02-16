# Lab 06 - Different Workloads

On this lab you'll use a [StatefulSet](https://kubernetes.io/docs/concepts/workloads/controllers/statefulset/) to control your database and a [CronJob](https://kubernetes.io/docs/concepts/workloads/controllers/cron-jobs/) to backup your database.

## On this lab

- [Prepare your machine](#prepare-your-machine)
- [Prepare your cluster](#prepare-your-cluster)
- [Create StatefulSet](#create-statefulset)
- [Update your secret](#update-your-secret)
- [Restart API deployment](#restart-api-deployment)
- [Test your application](#test-your-application)
- [Enable database backup](#enable-database-backup)
- [Check backup data](#enable-database-backup)

## Prepare your machine

First, enable `minikube` cluster on your machine.

```bash
minikube start --extra-config=kubelet.housekeeping-interval="10s"
```

## Prepare your cluster

**You should follow this step if you didn't finish [Lab04](/lab04.md) or cleared your cluster after that.**

Then create your first ingress, you need to install an ingress controller on your cluster.

```bash
minikube addons enable ingress
```

You need to check if ingress controller is already available on your cluster.

To do that, run the following command to check the status of the pod that implements the ingress controller.

```bash
kubectl get pods --namespace=ingress-nginx
```

You should get an output similar like this.

```bash
NAME                                        READY   STATUS      RESTARTS       AGE
ingress-nginx-admission-create--1-mzkx6     0/1     Completed   0              5h11m
ingress-nginx-admission-patch--1-4hzxw      0/1     Completed   0              5h11m
ingress-nginx-controller-54d8b558d4-jbnwx   1/1     Running     1 (103m ago)   5h11m
```

Your ingress controller is ready to be used when you see `Running` as the status of the pod with the name starting with `ingress-nginx-controller`.

Then, apply the following file to recreate all resources.

```bash
kubectl apply -f [echo-app-all.yaml](https://raw.githubusercontent.com/tasb/docker-kubernetes-training/main/src/EchoApp/manifests/sts/echo-app-all.yaml)
```

Now you're ready to continue to reconfigure your app.

## Create StatefulSet

You used a deployment as a controller for your database but you should use a StatefulSet since you need to have control about replicas created to handle databases.

At same time, with a StatefulSet you have a full match between a pod and a PersistentVolumeClaim.

Create a file named `echo-app-db.yaml` and add the following content.

```yaml
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: echo-db-sts
  namespace: echo-app-ns
spec:
  selector:
    matchLabels:
      app: echo-app
      tier: db
  serviceName: "echo-db-sts-svc"
  replicas: 1
  persistentVolumeClaimRetentionPolicy:
    whenDeleted: Retain
    whenScaled: Retain
  template:
    metadata:
      labels:
        app: echo-app
        tier: db
    spec:
      containers:
      - name: echo-db
        image: postgres:14.2-alpine
        ports:
        - containerPort: 1433
        env:
        - name: ACCEPT_EULA
          value: "Y"
        - name: SA_PASSWORD
          valueFrom:
            secretKeyRef:
              name: echo-api-db-secret
              key: dbpass
        volumeMounts:
        - name: echo-app-db-pv-claim
          mountPath: /var/opt/mssql/data
  volumeClaimTemplates:
  - metadata:
      name: echo-app-db-pv-claim
    spec:
      accessModes: [ "ReadWriteOnce" ]
      resources:
        requests:
          storage: 1Gi


---

apiVersion: v1
kind: Service
metadata:
  name: echo-db-sts-svc
  namespace: echo-app-ns
spec:
  ports:
    - port: 1433
      targetPort: 1433
      name: db
  selector:
    app: echo-app
    tier: db
  clusterIP: None

```

On this file you create a StatefulSet resource and the headless service needed to be used by pods to reach each pod of the StatefulSet.

Look that you create a PVC template that will automatically create a PVC and make the link between the pods and PVC.

Using this resource, you have a well defined structure for the names of all resources which allow you to handle more the dynamic behaviour of both resources.

Now you need to start change your app. During the next steps your application may me broken and not working properly.

First, delete database deployment.

```bash
kubectl delete deploy echo-db-dep -n echo-app-ns
```

Then, create StatefulSet and Headless Service.

```bash
kubectl apply -f echo-app-db.yaml
```

To confirm that your StatefulSet is running properly, you should list all StatefulSets on your cluster.

```bash
kubectl get sts -n echo-app-ns
```

You should get an output similar with this.

```bash
NAME          READY   AGE
echo-db-sts   1/1     5h15m
```

## Update your secret

Now you need to change your secret to reflect the new name of your server.

Using StatefulSet definition, your database is now located on `echo-db-sts-0.echo-db-sts-svc.echo-app-ns.svc.cluster.local`.

Then, you need to encode your connection string to Base64.

```bash
echo "Server=echo-db-sts-0.echo-db-sts-svc.echo-app-ns.svc.cluster.local,1433;Initial Catalog=echo-log;User ID=SA;Password=P@ssw0rd" | base64
```

Now, copy to the clipboard the result and let's update the secret already deployed.

```bash
kubectl edit secret echo-api-db-secret -n echo-app-ns
```

This command will open your prefered code editor on your machine. On the editor, you need to change the value of the key `.data.connString` with the value you copied to the clipboard.

When you close the code editor, the new definiton is automatically updated on your cluster.

## Restart API deployment

Since you mount the secret as an environment variable, you need to restart your pods to reflect the change.

You only need to run the following command for your pods to be recreated using a rolling update strategy.

```bash
kubectl rollout restart deploy/echo-api-dep -n echo-app-ns
```

You may check that your pods were restarted.

```bash
kubectl get pods -n echo-app-ns
```

And you should check that on column `AGE` you have time around few seconds.

## Test your application

Now your application should be working properly again.

You can open a browser and navigate to <http://echo-app.ingress.test> and test your Echo App!

## Enable database backup

To backup database data let's create a CronJob to run periodically to copy your database data to a target folder.

Start to create a file named `echo-app-backup.yml` and add the following content.

```yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: echo-app-db-backup
  namespace: echo-app-ns
spec:
  accessModes:
    - ReadWriteOnce
  resources:
    requests:
      storage: 3Gi

---

apiVersion: batch/v1
kind: CronJob
metadata:
  name: echo-app-db-backup
  namespace: echo-app-ns
spec:
  schedule: "* * * * *"
  jobTemplate:
    spec:
      template:
        metadata:
          labels:
            app: echo-app
            tier: backup
        spec:
          containers:
          - name: db-backup
            image: busybox
            args:
            - /bin/sh
            - -c
            - cp -r /backup/source/* /backup/target
            - sleep 3
            volumeMounts:
            - name: db-files
              mountPath: /backup/source
            - name: backup-folder
              mountPath: /backup/target
          restartPolicy: OnFailure
          volumes:
          - name: db-files
            persistentVolumeClaim:
              claimName: echo-app-db-pv-claim-echo-db-sts-0
          - name: backup-folder
            persistentVolumeClaim:
              claimName: echo-app-db-backup
```

On this file you create a PVC to handle your target folder to save database files and a CronJob to run perodically and copy files from one folder to another.

The job will run every minute due to `schedule: "* * * * *"`. You can use [crontab guru](https://crontab.guru/#*_*_*_*_*) to generate different schedules.

Let's create the cronjob.

```bash
kubectl apply -f echo-app-backup.yml -n echo-app-ns
```

Check if your CronJob where created properly.

```bash
kubectl get cj -n echo-app-ns
```

You should have an output similiar with this.

```bash
NAME                 SCHEDULE    SUSPEND   ACTIVE   LAST SCHEDULE   AGE
echo-app-db-backup   * * * * *   False     0        20s             86m
```

And finally, you should confirm if a pod ran successfully to copy the files.

```bash
kubectl get pods -n echo-app-ns -l tier=backup
```

And you should get a list with at least one pod with status `Completed`.

If you run this command several times, you can confirm that you have an history of 3 pods.

## Check backup data

Finally, let's confirm thay the backup was made properly.

Let's list all your PersistentVolumes and find out the name of the PV bound to the PVC named `echo-app-db-backup`.

```bash
PV_NAME=$(kubectl get pv -o jsonpath='{.items[?(@.spec.claimRef.name=="echo-app-db-backup")].metadata.name}')
```

You should get the name of the PV as the output of last command.

Now, you can get the path where the data is stored.

```bash
kubectl describe pv $PV_NAME>
```

On the output you get several details from the PV but you should focus on `Path` property.

You can enter minikube server running `minikube ssh` command and then navigate to the path you got from PV details and check if the folder is full of database files.

You just finish this lab and built a more stable definition of a database running on a Kubernetes Cluster.
