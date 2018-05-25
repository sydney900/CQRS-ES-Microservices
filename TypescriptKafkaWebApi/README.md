# Node Web Api for Kafka implemmented by Typescript


## How to use miniKube

### Install
miniKube download: https://github.com/kubernetes/minikube/releases

kubectl versions are available at a generic location as per the following format: https://storage.googleapis.com/kubernetes-release/release/${K8S_VERSION}/bin/${GOOS}/${GOARCH}/${K8S_BINARY}
https://storage.googleapis.com/kubernetes-release/release/stable.txt
https://storage.googleapis.com/kubernetes-release/release/v1.10.3/bin/windows/amd64/kubectl.exe

### Usage

.\minikube.exe version
.\minikube.exe --help
.\minikube.exe get-k8s-versions
.\minikube.exe start --help

.\minikube.exe start
.\minikube.exe status
.\kubectl.exe cluster-info
.\kubectl.exe version
.\minikube.exe ip
.\minikube.exe dashboard
.\minikube.exe dashboard --url=true

.\kubectl get nodes
.\kubectl.exe run hello-nginx --image=nginx --port=80
.\kubectl.exe get pods

.\kubectl.exe expose deployment hello-nginx --type=NodePort
.\kubectl.exe get services
.\kubectl.exe describe service hello-nginx

.\minikube.exe service --url=true hello-nginx
.\minikube.exe service hello-nginx

.\kubectl logs hello-nginx-2471083592-4vfz8

.\kubectl scale --replicas=3 deployment/hello-nginx
.\kubectl.exe get deployment


