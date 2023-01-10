# Docker Advanced Topics

## Docker compose

1. Show docker compose
    - Two services
    - Environment variables
    - Dependency

2. Run docker compose
```bash
docker compose up
```

## Lifecycle with docker compose

1. Open new terminal

2. Restart service
```bash
docker compose restart voting-db 
```

3. Stop all containers
```bash
docker compose down
```

## Dockerfile Multistage

1. Show EchoApp Dockerfiles