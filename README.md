# Kubernetes Basic to Advanced Course

On this repo you can find slides, demos and labs for Kubernetes Basic to Advanced Course.

If you prefer a web verions, you can navigate to <https://theonorg.github.io/k8s-basic2advanced-training/> to have access an HTML version of these instructions.

## On this page

- [Description](#description)
- [Requirements](#requirements)
- [Prepare your environment](#prepare-your-environment)
  - [Windows](#windows)
  - [Ubuntu](#ubuntu)
  - [macOS](#macos)
- [Labs](#labs)
- [ToDo App project](#todo-app-project)
- [Slides](#slides)
- [Feedback](#feedback)

## Description

Kubernetes is an open source platform for container orchestration and management. It automates the operations, administration, and deployment of containerized applications and services.

This course makes you learn how to use Kubernetes to build, deploy, and manage containers and cluster components in a secure and scalable environment.

By the end of this course, participants will be able to:

- Understand the architecture, core concepts, and components of a Kubernetes ecosystem.
- Set up, install, and configure a Kubernetes cluster for container orchestration.
- Learn how to execute Kubernetes operations using the command line tools.
- Get a hands-on experience from basic to advanced Kubernetes operations and administration.

## Requirements

- Conceptual understanding about containers
- Understand of containers lifecycle
- Experience with docker (or other container platform)
- Experience with using containers to deploy applications
- (Preferable) Experience with docker compose
- Familiarity with the Linux command line
- An understanding of networking concepts

## Prepare your environment

To perform the labs you need to have the following software installed on your machine.

### Windows

1. Windows 10+ (Windows 11 is recommended)
2. [Windows Terminal](https://www.microsoft.com/en-us/p/windows-terminal/9n0dx20hk701?activetab=pivot:overviewtab)
3. [Windows Subsystem for Linux](https://docs.microsoft.com/en-us/windows/wsl/install)
4. [Docker Desktop](https://www.docker.com/products/docker-desktop)
5. Configure WSL integration with Docker Desktop. More [here](https://docs.microsoft.com/en-us/windows/wsl/tutorials/wsl-containers#install-docker-desktop)
6. Install [Visual Studio Code](https://code.visualstudio.com/) (or other code editor of your preference)
7. Enable [Kubernetes on Docker Desktop](https://docs.docker.com/desktop/kubernetes/) (you may use any other kubernetes cluster at your choice)
8. (Optional) Some VS Code extension helpful for Docker and Kubernetes integration

    - [Docker](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-docker)
    - [Docker compose](https://marketplace.visualstudio.com/items?itemName=p1c2u.docker-compose)
    - [Kubernetes](https://marketplace.visualstudio.com/items?itemName=ms-kubernetes-tools.vscode-kubernetes-tools)
    - [YAML](https://marketplace.visualstudio.com/items?itemName=redhat.vscode-yaml)

### Ubuntu

1. Ubuntu 20.04
2. Docker. [How to install Docker Engine on Ubuntu](https://docs.docker.com/engine/install/ubuntu/)
3. Minikube. [How to install Minikube on Ubuntu](https://www.linuxtechi.com/how-to-install-minikube-on-ubuntu/)
4. Kubectl. [How to install Kubectl on Ubuntu](https://kubernetes.io/docs/tasks/tools/install-kubectl-linux/#install-using-native-package-management)
5. Install [Visual Studio Code](https://code.visualstudio.com/) (or other code editor of your preference)
6. (Optional) Some VS Code extension helpful for Docker and Kubernetes integration

    - [Docker](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-docker)
    - [Docker compose](https://marketplace.visualstudio.com/items?itemName=p1c2u.docker-compose)
    - [Kubernetes](https://marketplace.visualstudio.com/items?itemName=ms-kubernetes-tools.vscode-kubernetes-tools)
    - [YAML](https://marketplace.visualstudio.com/items?itemName=redhat.vscode-yaml)

This setup works on top of [Windows Subsystem for Linux](https://docs.microsoft.com/en-us/windows/wsl/install).

### macOS

1. Docker. [Install Docker Desktop on Mac](https://docs.docker.com/desktop/install/mac-install/)
2. Minikube. [How to install Minikube on Mac](https://minikube.sigs.k8s.io/docs/start/)
3. Kubectl. [How to install Kubectl on Mac](https://kubernetes.io/docs/tasks/tools/install-kubectl-macos/)
4. Install [Visual Studio Code](https://code.visualstudio.com/docs/setup/mac) (or other code editor of your preference)
5. (Optional) Some VS Code extension helpful for Docker and Kubernetes integration

    - [Docker](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-docker)
    - [Docker compose](https://marketplace.visualstudio.com/items?itemName=p1c2u.docker-compose)
    - [YAML](https://marketplace.visualstudio.com/items?itemName=redhat.vscode-yaml)

## Labs

On next links you may find the hands-on exercises to give you more experience on this topics.

You may navigate for each one individually or you may follow the sequence starting on first one and proceed to the next using the navigation links at the end of each lab.

1. [Introduction to Kubernetes](labs/lab01.md)
2. [Deployment lifecycle](labs/lab02.md)
3. [Managing services](labs/lab03.md)
4. [Storage in Kubernetes](labs/lab04.md)
5. [Auto-Scalling on your cluster](labs/lab05.md)
6. [Different Workloads](labs/lab06.md)
7. [Manage your pods connections](labs/lab07.md)
8. [Use Helm to manage your manifests](labs/lab08.md)

## ToDo App project

With this simple ToDo App you have the hands-on experience to create all needed artifacts to deploy an app on Kubernetes.

- Step #1 [Create Kubernetes manifests and run on your cluster](project/step01.md)

## Slides

Get access to the content used to share Kubernetes concepts during sessions.

1. [Container orchestration](slides/Session01.pdf)
2. [Introduction to Kubernetes](slides/Session02.pdf)
3. [Deployment lifecycle](slides/Session03.pdf)
4. [Managing services](slides/Session04.pdf)
5. [Storage in Kubernetes](slides/Session05.pdf)
6. [Scaling and Probes](slides/Session06.pdf)
7. [Different Workloads](slides/Session07.pdf)
8. [Secure your cluster](slides/Session08.pdf)
9. [Helm](slides/Session09.pdf)
10. [Monitor your cluster and your application](slides/Session10.pdf)

## Feedback

For any feedback open up an issue describing what have you found and I'll return to you!
