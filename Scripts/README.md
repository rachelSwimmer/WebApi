# K6 Load Testing Scripts

## Scripts

1. **simple-test.js** - Basic POST request
2. **load-test.js** - Load test with stages and thresholds
3. **api-load-test.js** - Test all API endpoints

## Installation

```bash
# Install k6
# Ubuntu/Debian
sudo gpg -k
sudo gpg --no-default-keyring --keyring /usr/share/keyrings/k6-archive-keyring.gpg --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys C5AD17C747E3415A3642D57D77C6C491D6AC1D69
echo "deb [signed-by=/usr/share/keyrings/k6-archive-keyring.gpg] https://dl.k6.io/deb stable main" | sudo tee /etc/apt/sources.list.d/k6.list
sudo apt-get update
sudo apt-get install k6

# Or using snap
sudo snap install k6
```

## Running Tests

```bash
# Run simple test
k6 run Scripts/simple-test.js

# Run load test with stages
k6 run Scripts/load-test.js

# Run API load test
k6 run Scripts/api-load-test.js

# Run with custom virtual users and duration
k6 run --vus 10 --duration 30s Scripts/simple-test.js

# Run with iterations
k6 run --iterations 100 Scripts/simple-test.js
```

## Test Results

k6 will show:
- Response times (min, avg, max, p95, p99)
- Request rate (requests per second)
- Failed requests
- Data sent/received
- Iteration duration

## Comparing Async vs Sync

```bash
# Test the compare endpoint to see async benefits
curl http://localhost:5081/api/examples/compare
```

This will show how async/await performs better under load!
