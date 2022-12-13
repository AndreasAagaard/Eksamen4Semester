docker build -t nicolaiskat/eksamen-bidding-publisher:latest -f Dockerfile .
docker push nicolaiskat/eksamen-bidding-publisher:latest
docker tag nicolaiskat/eksamen-bidding-publisher:latest ffrepo.azurecr.io/eksamen-bidding-publisher:v0.9.0
docker push ffrepo.azurecr.io/eksamen-bidding-publisher:v0.9.0