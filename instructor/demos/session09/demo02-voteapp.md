helm lint voting-app/

helm template --debug voting-app/

helm install --dry-run --debug --generate-name voting-app

helm install voting-app voting-app/

kubectl get pods

helm get manifest voting-app

UPDATE Chart

helm upgrade voting-app voting-app/

PASTE release notes

helm test voting-app