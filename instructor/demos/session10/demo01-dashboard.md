# Kubernetes Dashboard Demo

## Open Kubernetes

```bash
kubectl port-forward -n kube-system service/kubernetes-dashboard 10443:443 --address 0.0.0.0
```

## Get token for login

1. Use admin user token on your kubeconfig

```bash
cat ~/.kube/config
```

2. Copy admin token and paste on Kubernetes Dashboard login

## Navigate on Dashboard

1. List namespaces
2. List pods (see resource usage charts)
3. Show how to create a new resource
4. Select a deployment and show how to scale
