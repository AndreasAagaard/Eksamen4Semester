docker build -t nicolaiskat/eksamen-auction-service:latest -f Dockerfile .
docker push nicolaiskat/eksamen-auction-service:latest
docker tag nicolaiskat/eksamen-auction-service:latest ffrepo.azurecr.io/eksamen-auction-service:v0.9.0
docker push ffrepo.azurecr.io/eksamen-auction-service:v0.9.0