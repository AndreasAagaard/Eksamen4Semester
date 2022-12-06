#!/bin/bash
# A sample Bash script, by Ryan
cd ../auction-solution/auction-service
docker build -t eksamen-auction-service:dev -f Dockerfile .
cd ../../item-solution/item-service
docker build -t eksamen-item-service:dev -f Dockerfile .
cd ../../BiddingHandler/BidddingHandler
docker build -t eksamen-bidding-service:dev -f Dockerfile .