import { WebSocketServer } from 'ws';

export class WebSocket {
  _socket;
  _port;
  _interval;

  constructor(server) {
    this._socket = new WebSocketServer({ server });

    this.handleEvents();
  }

  handleEvents() {
    this._socket.on('connection', (socket) => {
      console.log('New client connected');
      socket.send('Connected');

      if (!this._interval) {
        this._interval = setInterval(() => {
          const message = 'Hello from server!';
          socket.send(message);
        }, 5000);
      }

      socket.on('message', (message) => {
        console.log(`Received message: ${message}`);
        socket.send(`Server received: ${message}`);
      });

      socket.on('close', () => {
        console.log('Client disconnected');
        clearInterval(this._interval);
        this._interval = null;
      });
    });
  }
}
