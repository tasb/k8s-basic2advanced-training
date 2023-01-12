FROM python:3.11.1-slim-bullseye as python-redis
RUN apt-get update && apt-get install -y redis-server
EXPOSE 80

FROM python-redis as voting-prep
WORKDIR /app
ADD requirements.txt /app/requirements.txt
RUN pip install -r requirements.txt

FROM voting-prep as voting-final
LABEL org.opencontainers.image.source=https://github.com/theonorg/k8s-basic2advanced-training
ADD . /app
RUN chmod +x /app/start.sh
ENTRYPOINT "/app/start.sh"
