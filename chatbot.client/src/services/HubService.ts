import * as signalR from "@microsoft/signalr";
import { HubMessageModel } from "../models/HubMessageModel";
import { HubAddMessageModel } from "../models/HubAddMessageModel";
import { AccesTokenService } from "./AccessTokenService";

class HubService {
  private hubConnection: signalR.HubConnection | null = null;

  public startConnection = (url: string) => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url, { accessTokenFactory: () => AccesTokenService.getAccessToken()! })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
  }

  public addMessageListener = (callback: (message: HubMessageModel) => void) => {
    if (this.hubConnection) {
      this.hubConnection.on('ReceiveMessage', (message: HubMessageModel) => {
        callback(message);
      });
    }
  }

  public removeMessageListener = () => {
    if (this.hubConnection) {
      this.hubConnection.off('ReceiveMessage');
    }
  }

  public sendMessage = (message: HubAddMessageModel) => {
    if (this.hubConnection) {
      this.hubConnection.invoke('AddMessage', message);
    }
  }
}

export const hubService = new HubService();