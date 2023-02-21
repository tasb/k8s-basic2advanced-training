kubectl config set-cluster my-cluster --server=https://kubernetes.default --certificate-authority=/var/run/secrets/kubernetes.io/serviceaccount/ca.crt

kubectl config set-context my-cluster --cluster=my-cluster

kubectl config set-credentials user --token=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)

kubectl config set-context my-cluster --user=user

kubectl config use-context my-cluster