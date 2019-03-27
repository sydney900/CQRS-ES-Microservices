# How to 

## Create AKS cluster and set up to run KubeCtl
---
1. Install Azure Cli then run command to login 
```
az login
```
2. Run command below to create resource group
```
az group create --name myKuberGroup --location australiaeast
```
3. Run command below to create a Azure Kubernetes cluster
```
az aks create --resource-group myKuberGroup --name myFirstCluster --node-vm-size Standard_B2s --node-count 1 --enable-addons monitoring --generate-ssh-keys
```
4. Run command below to install AKS kubectl
```
az aks install-cli
```
5. Set path for running KubeCtl
```
$env:path += 'C:\Users\bingw\.azure-kubectl'
```
6. Get credential to run kubectl for AKS cluster
```
az aks get-credentials --resource-group myKuberGroup  --name myFirstCluster
```

## Run AKS cluster dashpbard
---
```
az aks browse --resource-group MyKuberGroup  --name MyFirstCluster
```
When RBAC was enabled, below erro will happen
persistentvolumeclaims is forbidden: User "system:serviceaccount:kube-system:kubernetes-dashboard" cannot list persistentvolumeclaims in the namespace "default" 
then run following command to fix this
```
kubectl create clusterrolebinding kubernetes-dashboard --clusterrole=cluster-admin --serviceaccount=kube-system:kubernetes-dashboard
```

## How to use hlem
---
1. Download helm client
2. Set path for running helm client
3. Run command below to set helm server
```
helm init --tiller-tls-verify --kube-context myFirstCluster
```
4. Edit helm charts, example in subfolder V1 and V1Helm for version 1 microservices. V1Helm folder is latest.
5. Run command to install helm charts
command below for check the charts
```
helm install V1Helm --dry-run --debug
```
command below for installing the charts
```
helm install V1Helm
```
```
helm install V1Helm -n v1-2019-02-01
```
v1-2019-02-01 is release name
6. Update helm charts
```
helm upgrade v1-2019-02-01 V1Helm
```
```
helm upgrade v1-2019-02-01 V1Helm --dry-run --debug
```

## How to delete all objects in a cluster
---
1. Method one
```
powershell helm del $(helm ls --all --short) --purge
```
2. Method two
```
kubectl delete deployments --all
kubectl delete pods --all
kubectl delete services --all
```

## How to use nginx ingress in Kubernetes cluster
---
1. install nginx
```
helm install --name v1-2019-02-01 stable/nginx-ingress
```
2. install cert-manager
* install the cert-manager CRDs
```
kubectl apply -f https://raw.githubusercontent.com/jetstack/cert-manager/release-0.6/deploy/manifests/00-crds.yaml
```
* make sure the namespace has an additional label
```
kubectl label namespace default certmanager.k8s.io/disable-validation="true"
```
* install the cert-manager helm chart
```
helm install --name v1-2019-02-01-02 stable/cert-manager
```
3. Setup Certificate Cluster Issuer
kubectl apply -f cluster-issuer-staging.yaml

4. Install the microservice
helm install V1Helm -n v1-2019-02-01-svc
helm upgrade v1-2019-02-01-svc V1Helm 

## Other usefull commands
1. Disable Kubernetes RBAC
```
kubectl create clusterrolebinding permissive-binding --clusterrole=cluster-admin --user=admin --user=kubelet --group=system:serviceaccounts;
```
2. Powershell command to instald kompose
```
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
Invoke-WebRequest -Uri https://github.com/kubernetes/kompose/releases/download/v1.16.0/kompose-windows-amd64.exe
```
3. Fix Error: configmaps is forbidden: User "system:serviceaccount:kube-system:default" cannot list configmaps in the namespace "kube-system"
```
kubectl create serviceaccount --namespace kube-system tiller
kubectl create clusterrolebinding tiller-cluster-rule --clusterrole=cluster-admin --serviceaccount=kube-system:tiller
kubectl patch deploy --namespace kube-system tiller-deploy -p '{"spec":{"template":{"spec":{"serviceAccount":"tiller"}}}}'      
helm init --service-account tiller --upgrade
```
4. Fix connection refused
Addin the enviroment variable in deployment yaml file
          env:
            - name: NETWORK_HOST
              value: "eth0:ipv4"

## How to set up dashboard for local kubernetes
1. install dashboard for local kubernetes
Got to subfoler "LocalYaml" and run run.bat
2. Run dashboard of a local kubernetes
Run command below
```
kubectl proxy
```
and then launch URL below to show dashboard

http://localhost:8001/api/v1/namespaces/kube-system/services/https:kubernetes-dashboard:/proxy/

3. the command examples to get credential to view dashboard
```
kubectl -n default get secret
kubectl -n default describe secret default-token-75ccr

kubectl -n kube-system get secret
kubectl -n kube-system describe secret sydney-token-g9nwl
```

### One command to push all lates images to docker hub:
---
```
docker image tag websitenokafka:latest sydney900/websitenokafka:latest

docker push sydney900/websitenokafka:latest

docker image tag apigateway:latest sydney900/apigateway:latest

docker push sydney900/apigateway:latest

docker image tag authserver:latest sydney900/authserver:latest

docker push sydney900/authserver:latest

docker image tag clientservice:latest sydney900/clientservice:latest

docker push sydney900/clientservice:latest

docker image tag productservice:latest sydney900/productservice:latest

docker push sydney900/productservice:latest
```