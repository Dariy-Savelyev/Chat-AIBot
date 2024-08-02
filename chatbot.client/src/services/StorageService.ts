export class StorageService {
    static readonly accessToken : string = 'accessToken';
    static readonly lastRefreshAccessTokenDate : string = 'lastRefreshAccessTokenDate';

    static saveAccessToken(accessToken: string) {
        localStorage.setItem(StorageService.accessToken, accessToken);
    }

    static getAccessToken(): string | null {
        return localStorage.getItem(StorageService.accessToken);
    }

    static saveLastRefreshAccessTokenDate(date: string) {
        localStorage.setItem(StorageService.lastRefreshAccessTokenDate, date);
    }

    static getLastRefreshAccessTokenDate(): string | null {
        return localStorage.getItem(StorageService.lastRefreshAccessTokenDate);
    }

    static revokeAccessToken() {
        localStorage.removeItem(StorageService.accessToken);
        localStorage.removeItem(StorageService.lastRefreshAccessTokenDate);
    }
}