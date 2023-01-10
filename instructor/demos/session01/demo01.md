# Docker Intro commands

## Run a container 
```bash
docker run -d --name voting-app -p 9000:80 tasb/voting-app:full
```

## Stop container
```bash
docker stop voting-app
```

## Start container
```bash
docker start voting-app
```

Check that data still exists on Redis

## Remove container
```bash
docker rm -f voting-app
```

## Start new container
```bash
docker run -d --name voting-app -p 9000:80 tasb/voting-app:full
```

Check data were reset