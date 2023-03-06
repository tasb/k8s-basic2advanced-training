helm repo add bitnami https://charts.bitnami.com/bitnami

helm search repo

helm search repo nginx -l

helm pull bitnami/nginx --version 13.2.16

helm inspect readme bitnami/nginx

helm install nginx bitnami/nginx  --version 13.2.16 -n nginx --create-namespace

kubectl get all -n nginx

helm list

helm list -n nginx

helm list -A

helm upgrade nginx bitnami/nginx  --version 13.2.16 -n nginx --set image.tag=latest

kubectl get all -n nginx

helm inspect values bitnami/nginx > values_all.yaml

helm upgrade nginx bitnami/nginx -n nginx --values values.yaml

kubectl get all -n nginx

helm list -A

helm history nginx -n nginx

helm rollback nginx -n nginx <NUMBER>

helm delete nginx  -n nginx

helm list -A

kubectl get all -n nginx

kubectl delete ns nginx
