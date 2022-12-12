docker build -t nicolaiskat/eksamen-item-service:latest -f Dockerfile .
docker push nicolaiskat/eksamen-item-service:latest
docker tag nicolaiskat/eksamen-item-service:latest ffrepo.azurecr.io/eksamen-item-service:v0.9.0
docker push ffrepo.azurecr.io/eksamen-item-service:v0.9.0