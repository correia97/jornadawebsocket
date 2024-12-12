FROM ubuntu:22.04
RUN apt-get update && \
    apt-get install -y unzip curl

RUN curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" &&\
            unzip awscliv2.zip &&\
            ./aws/install    

WORKDIR /script
COPY . .

CMD /script/init-awslocal.sh