# Create your Helm Charts

On this lab you'll create Helm Charts for Echo App and make it simpler to manage Kubernetes manifests.

## On this lab

- [Prepare your machine](#prepare-your-machine)
- [Install Helm](#install-helm)
- [Get Echo App Helm package](#get-echo-app-helm-package)
- [Execute Echo App DB Helm Chart](#execute-echo-app-db-helm-chart)
- [Execute Echo App API Helm Chart](#execute-echo-app-api-helm-chart)
- [Execute Echo App WebApp Helm Chart](#execute-echo-app-webapp-helm-chart)

## Prepare your machine

First, enable `minikube` cluster on your machine.

```bash
minikube start --extra-config=kubelet.housekeeping-interval="10s"
```

Check if you already have Echo App running and if so, let's delete it since we'll recreate everything from the scratch using Helm Charts.

Check if you have `echo-app-ns` namespace on your cluster.

```bash
kubectl get ns
```

If you see `echo-app-ns` on returning list, you should delete it. If not, you may skip this step.

To delete a namespace, you run the following command.

```bash
kubectl delete ns echo-app-ns
```

## Install Helm

You may follow the installation steps describe on [Helm Website](https://helm.sh/docs/intro/install/) that have several options, depending of the operating system you're using.

For Ubuntu, you may run the following commands:

```bash
curl https://baltocdn.com/helm/signing.asc | gpg --dearmor | sudo tee /usr/share/keyrings/helm.gpg > /dev/null
sudo apt-get install apt-transport-https --yes
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/helm.gpg] https://baltocdn.com/helm/stable/debian/ all main" | sudo tee /etc/apt/sources.list.d/helm-stable-debian.list
sudo apt-get update
sudo apt-get install helm
```

After installation finish with success, you try the following command to check if Helm is working properly.

```bash
helm version
```

## Get Echo App Helm package

If you're creating your Helm Charts from the scratch, you should start by scaffold the standard folder structure of Helm Chart with `helm create` command.

After that, you may start to adapt all the files to fit with your application.

To make these steps faster, you can download final version of Helm Charts files from next link: <https://github.com/theonorg/k8s-basic2advanced-training/files/10949817/echo-app-helm-charts.zip>

Create a folder named `deploy` and unzip the contents of the previously downloaded zip file into that folder.

## Review Helm Charts

Now, take some time to review all files.

First, check `Chart.yaml` files, where you can set Chart metadata, like a description and chart and application version.

Then, check `values.yaml` files. Those are crucial for you to understand all the variables you can set when installing or upgrading each Helm Chart.

You can even see clearly the difference between the amount of possible values to be configured from Database component to the API and WebApp component.

The first uses a more restricted approach, the latter a more open approach being able to even decide if you want to have HorizontalPodAutoscaler and Ingress configured.

Then, take a look on all templates folders, where you may find templates for all Kubernetes resources needed for each component.

You may find the following Kubernetes templates:

- `echo-app-db` component:
  - StatefulSet
  - Secret
  - Service
- `echo-app-api` component:
  - Deployment
  - Secret
  - Service
  - HorizontalPodAutoscaler
  - Ingress
- `echo-app-db` component:
  - Deployment
  - Secret
  - Service
  - HorizontalPodAutoscaler
  - Ingress

Inside each `templates` folder you may find a `_helpers.tpl` file where some functions are defined that can be used on your templates.

Additionally you have a `NOTES.txt` file that is used to be displayed when you finalize with success the installation or upgrading of a helm release.

Finally, on `echo-app-api` and `echo-app-web` folder, you may find a `tests` folder where a simple template allow you to create a pod inside your cluster to test if each component.

Take your time to go through these files and to understand how the templates can help you creating dynamic manifest files.

Let's check some specific portions to highlight great feature from Helm.

On file `echo-app-api/templates/_helpers.tpl` file you may find this block of code.

```yaml
{{- define "echo-app-api.connString" -}}
{{- printf "%s%s%s%s%s%s" "Server=" .Values.dbHost ",1433;Initial Catalog=echo-log;User ID=" .Values.dbUser ";Password=" .Values.dbPass }}
{{- end }}
```

This is a way that you can create a string using same static portions and variables set on your `values.yaml` file.

On file `echo-app-api/templates/secret.yaml`, you have this code.

```bash
connString: {{ include "echo-app-api.connString" . | b64enc }}
```

On this portion, you're using the previous defined function and using a built-in function named `b64enc` that automatically encodes a string into Base64 format, following the rules defined for the secrets in Kubernetes.

Next, on file `echo-app-api/templates/ingress.yaml`, you may check that your file starts with the following 3 lines.

```bash
{{- if .Values.ingress.enabled -}}
{{- $fullName := include "echo-app-api.fullname" . -}}
{{- $svcPort := .Values.service.port -}}
```

First line works as a conditional. Will check if the property `ingress.enabled` on `values.yaml` file is set to true. Only on that case, the file will be produced. Otherwise, an empty file is created.

The next two lines you're setting 2 variables (`fullName` nad `svcPort`) with values from `_helpers.tpl` file and `values.yaml` file respectively.

To use the defined variables, you may check the same file on line 25, where `svcPort` variable is used.

```yaml
number: {{ $svcPort }}
```

Continuing on this file, you may find how to execute a cycle on Helm.

```yaml
{{- range .Values.ingress.paths }}
  - path: {{ .path }}
    pathType: {{ .pathType }}
    backend:
      service:
        name: {{ $fullName }}-svc
        port:
          number: {{ $svcPort }}
{{- end }}
```

The initial line of code, starts a cycle that will iterate for all objects defined on the list `ingress.paths` on `values.yml`.

This approach allow you to define several paths to be included on the ingress definition only by creating a new `values.yaml` file and to give it as parameter for helm installation or upgrade.

Finally, on file `echo-app-db/templates/sts.yaml` you may find these lines.

```yaml
  persistentVolumeClaimRetentionPolicy:
    whenDeleted: {{ .Values.retentionPolicy.delete | default "Retain" }}
    whenScaled: {{ .Values.retentionPolicy.scale | default "Retain" }}
```

Where you can verify that a `default` value is set for both properties making that the properties on `values.yaml` file are not required.

## Execute Echo App DB Helm Chart

To install Echo App DB Helm Chart, we will only set `dbPass` value. Everything else, will be defined with default values.

Run the following command to install `echo-app-db` helm chart.

```bash
helm install echo-app-db echo-app-db/ -n echo-app-ns --create-namespace --set dbPass=P@ssw0rd
```

On this command, you create a new Helm Release named `echo-app-db` using the helm chart defined on folder `echo-app-db`.

After you run successfully this command, you should see the contents of `NOTES.txt` file with template blocks replace with values used for these release.

The deploy will be made on namespace `echo-app-ns` that you just deleted and to not send any error, you set the flag  `--create-namespace` to create (if needed) the defined namespace.

Now you can check you have your Helm Release.

```bash
helm list -n echo-app-ns
```

And get an output similar with this. Pay attention on the column `STATUS` where you should have a `deployed` value.

```bash
NAME        	NAMESPACE  	REVISION	UPDATED                                	STATUS  	CHART             	APP VERSION
echo-app-db 	echo-app-ns	1       	2023-03-11 00:01:21.940402012 +0000 UTC	deployed	echo-app-db-0.5.0 	0.1.0
```

After, release is made, you can use `kubectl` commands like you have used on all the other labs.

```bash
kubectl get pods -n echo-app-ns
```

And check you have one pod running your Echo App Database.

## Execute Echo App API Helm Chart

To install Echo App API Helm Chart, we will set `dbPass` value and enable ingress to be able to make direct requests to the API. Everything else, will be defined with default values.

Run the following command to install `echo-app-api` helm chart.

```bash
helm install echo-app-api echo-app-api/ -n echo-app-ns --create-namespace --set dbPass=P@ssw0rd --set ingress.enabled=true
```

On this command, you create a new Helm Release named `echo-app-api` using the helm chart defined on folder `echo-app-api`.

After you run successfully this command, you should see the contents of `NOTES.txt` file with template blocks replace with values used for these release.

The deploy will be made on namespace `echo-app-ns` that was created on previous helm command. Using the flag  `--create-namespace` can be always done since only creates the namespace if needed.

Now you can check you have your Helm Release.

```bash
helm list -n echo-app-ns
```

And get an output similar with this. Pay attention on the column `STATUS` where you should have a `deployed` value on both releases.

```bash
NAME        	NAMESPACE  	REVISION	UPDATED                                	STATUS  	CHART             	APP VERSION
echo-app-api	echo-app-ns	2       	2023-03-12 00:06:25.986231865 +0000 UTC	deployed	echo-app-api-1.0.0	1.0.0
echo-app-db 	echo-app-ns	1       	2023-03-11 00:01:21.940402012 +0000 UTC	deployed	echo-app-db-0.5.0 	0.1.0
```

After, release is made, you can use `kubectl` commands like you have used on all the other labs.

```bash
kubectl get pods -n echo-app-ns
```

And check you have one pod running your Echo App Database and one pod running your Echo App API.

On this Helm Chart you have defined a test pod so you can run it to confirm your deploy was done successfully.

```bash
helm test echo-app-api -n echo-app-ns
```

The command can take some time to be performed since a pod needs to be created. After the execution, you should see an output like this.

```bash
NAME: echo-app-api
LAST DEPLOYED: Sun Mar 12 00:06:25 2023
NAMESPACE: echo-app-ns
STATUS: deployed
REVISION: 2
TEST SUITE:     echo-app-api-test-connection
Last Started:   Sun Mar 12 16:45:23 2023
Last Completed: Sun Mar 12 16:45:27 2023
Phase:          Succeeded
NOTES:
1. To check if your service is running properly you need to run a port-forward command:
  kubectl --namespace echo-app-ns port-forward svc/echo-app-api-svc 8080:8080
  echo "Visit http://127.0.0.1:8080 to use your application"
```

You should focus on line `Phase: Succeeded` to have sure that your test ran with success.

Since you enabled the ingress resource, you can make the first test using your browser.

Open a browser window and navigate to <http://echo-app.ingress.test/api/hostname> to get the hostname of your API pod.

## Execute Echo App WebApp Helm Chart

Finally, let's create Echo App WebApp Helm chart.

To install Echo App WebApp Helm Chart, we will enable ingress to be able to use the webapp using a proper DNS. Everything else, will be defined with default values.

Run the following command to install `echo-app-web` helm chart.

```bash
helm install echo-app-web echo-app-web/ -n echo-app-ns --create-namespace --set ingress.enabled=true
```

On this command, you create a new Helm Release named `echo-app-web` using the helm chart defined on folder `echo-app-web`.

After you run successfully this command, you should see the contents of `NOTES.txt` file with template blocks replace with values used for these release.

The deploy will be made on namespace `echo-app-ns` that was created on previous helm command. Using the flag  `--create-namespace` can be always done since only creates the namespace if needed.

Now you can check you have your Helm Release.

```bash
helm list -n echo-app-ns
```

And get an output similar with this. Pay attention on the column `STATUS` where you should have a `deployed` value on both releases.

```bash
NAME        	NAMESPACE  	REVISION	UPDATED                                	STATUS  	CHART             	APP VERSION
echo-app-api	echo-app-ns	2       	2023-03-12 00:06:25.986231865 +0000 UTC	deployed	echo-app-api-1.0.0	1.0.0
echo-app-db 	echo-app-ns	1       	2023-03-11 00:01:21.940402012 +0000 UTC	deployed	echo-app-db-0.5.0 	0.1.0
echo-app-web	echo-app-ns	2       	2023-03-12 00:25:25.310538245 +0000 UTC	deployed	echo-app-web-1.0.0	1.0.0
```

After, release is made, you can use `kubectl` commands like you have used on all the other labs.

```bash
kubectl get pods -n echo-app-ns
```

And check you now have one pod running your Echo App Database, another pod running your Echo App API and another pod running your Echo App WebApp.

Now that you've created all needed resources, execute some additional `kubectl` commands to check all created resources.

```bash
kubectl get all -n echo-app-ns

kubectl get ingress -n echo-app-ns

kubectl get secret -n echo-app-ns
```

On this Helm Chart you have defined a test pod so you can run it to confirm your deploy was done successfully.

```bash
helm test echo-app-web -n echo-app-ns
```

The command can take some time to be performed since a pod needs to be created. After the execution, you should see an output like this.

```bash
NAME: echo-app-web
LAST DEPLOYED: Sun Mar 12 00:25:25 2023
NAMESPACE: echo-app-ns
STATUS: deployed
REVISION: 2
TEST SUITE:     echo-app-web-test-connection
Last Started:   Sun Mar 12 16:56:33 2023
Last Completed: Sun Mar 12 16:56:37 2023
Phase:          Succeeded
NOTES:
1. Get the application URL by running these commands:

  http://echo-app.ingress.test/(.*)
```

You should focus on line `Phase: Succeeded` to have sure that your test ran with success.

Since you enabled the ingress resource, you can make the first test using your browser.

Open a browser window and navigate to <http://echo-app.ingress.test/> to get access to Echo App and starting using it!

Congratulation! You just configured Echo App on your cluster using Helm Charts to help on this task and creating several packages that can be easily deployed on another cluster/namespace.
