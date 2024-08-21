import { ACCESS_TOKEN_VALIDITY_SECONDS } from "../utils/Constants";
import { StorageService as storage } from "./StorageService";

export class AccesTokenService {
    static saveAccessToken(accessToken: string) {
        storage.saveLastRefreshAccessTokenDate(new Date().getTime().toString());
        storage.saveAccessToken(accessToken);
    }

    static getAccessToken(): string | null {
        return storage.getAccessToken();
    }

    static revokeAccessToken() {
        storage.revokeAccessToken();
    }

    static isExpired(): boolean {
        let currentTime = new Date().getTime();
        let lastRefreshAccessTokenTime = + storage.getLastRefreshAccessTokenDate()!;
        
        if ((currentTime - lastRefreshAccessTokenTime) > ((ACCESS_TOKEN_VALIDITY_SECONDS - 60) * 1000)) {
            return true;
        }

        return false;
    }

    static isLoggedIn(): boolean {
        const token = storage.getAccessToken();
        const isLoggedIn = token != null;

        return isLoggedIn;
    }
}