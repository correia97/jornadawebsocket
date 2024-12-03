#!/bin/sh

# set region globally
export AWS_DEFAULT_REGION=sa-east-1

# install `jq`, if not yet available
which jq || apt-get -y install jq


export REST_API_ID=12345

# create rest api gateway
echo "Create Rest API..."
awslocal apigateway create-rest-api --name quote-api-gateway --tags '{"_custom_id_":"12345"}'

# get parent id of resource
echo "Export Parent ID..."
export PARENT_ID=$(awslocal apigateway get-resources --rest-api-id $REST_API_ID | jq -r '.items[0].id')pi

# get resource id
echo "Export Resource ID..."
export RESOURCE_ID=$(awslocal apigateway create-resource --rest-api-id $REST_API_ID --parent-id $PARENT_ID --path-part "productApi" | jq -r '.id')

echo "RESOURCE ID: $RESOURCE_ID"

# echo "Put GET Method..."
# awslocal apigateway put-method \
# --rest-api-id $REST_API_ID \
# --resource-id $RESOURCE_ID \
# --http-method GET \
# --request-parameters "method.request.path.productApi=true" \
# --authorization-type "NONE"

# echo "Put POST Method..."
# awslocal apigateway put-method \
# --rest-api-id $REST_API_ID \
# --resource-id $RESOURCE_ID \
# --http-method POST \
# --request-parameters "method.request.path.productApi=true" \
# --authorization-type "NONE"


# echo "Update GET Method..."
# awslocal apigateway update-method \
#   --rest-api-id $REST_API_ID \
#   --resource-id $RESOURCE_ID \
#   --http-method GET \
#   --patch-operations "op=replace,path=/requestParameters/method.request.querystring.param,value=true"


# echo "Put POST Method Integration..."
# awslocal apigateway put-integration \
#   --rest-api-id $REST_API_ID \
#   --resource-id $RESOURCE_ID \
#   --http-method POST \
#   --type AWS_PROXY \
#   --integration-http-method POST \
#   --uri arn:aws:apigateway:$AWS_DEFAULT_REGION:lambda:path/2015-03-31/functions/arn:aws:lambda:$AWS_DEFAULT_REGION:000000000000:function:add-product/invocations \
#   --passthrough-behavior WHEN_NO_MATCH

# echo "Put GET Method Integration..."
# awslocal apigateway put-integration \
#   --rest-api-id $REST_API_ID \
#   --resource-id $RESOURCE_ID \
#   --http-method GET \
#   --type AWS_PROXY \
#   --integration-http-method GET \
#   --uri arn:aws:apigateway:$AWS_DEFAULT_REGION:lambda:path/2015-03-31/functions/arn:aws:lambda:$AWS_DEFAULT_REGION:000000000000:function:get-product/invocations \
#   --passthrough-behavior WHEN_NO_MATCH

echo "Create DEV Deployment..."
awslocal apigateway create-deployment \
  --rest-api-id $REST_API_ID \
  --stage-name dev

awslocal sns create-topic --name webhook-topic

awslocal sqs create-queue --queue-name webhook-queue

awslocal sqs get-queue-attributes --queue-url http://localhost:4566/000000000000/webhook-queue --attribute-names QueueArn

awslocal sns subscribe \
    --topic-arn arn:aws:sns:$AWS_DEFAULT_REGION:000000000000:webhook-topic \
    --protocol sqs \
    --notification-endpoint arn:aws:sqs:$AWS_DEFAULT_REGION:000000000000:webhook-queue

awslocal sqs set-queue-attributes \
    --queue-url http://localhost:4566/000000000000/webhook-queue \
    --attributes VisibilityTimeout=10

