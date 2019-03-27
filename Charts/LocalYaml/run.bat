kubectl apply -f dashboard-adminuser.yaml
kubectl apply -f ClusterRoleBinding.yaml
kubectl -n kube-system describe secret $(kubectl -n kube-system get secret | grep admin-user | awk '{print $1}')