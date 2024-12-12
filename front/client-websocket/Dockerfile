FROM node:20 as build

WORKDIR /app

COPY package.json package.json
COPY package-lock.json package-lock.json

RUN npm install

COPY . .

RUN npm run build -c Production

FROM nginx:alpine
ENV TZ=America/Sao_Paulo
RUN cat /usr/share/zoneinfo/$TZ > /etc/localtime \
		&& cat /usr/share/zoneinfo/$TZ > /etc/timezone \
		&& update-ca-certificates \
        rm -rf /var/lib/apt/lists/*

COPY --from=build /app/default.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist/client-websocket/browser /usr/share/nginx/html

# Expose the default nginx port
EXPOSE 80
ENV PORT=80
CMD ["nginx", "-g", "daemon off;"]