kubectl port-forward -n observability service/kube-prom-stack-grafana --address 0.0.0.0 9080:80

kubectl port-forward -n observability service/kube-prom-stack-kube-prome-prometheus --address 0.0.0.0 9090:9090