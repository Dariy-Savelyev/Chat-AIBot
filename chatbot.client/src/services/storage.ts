export class storage {
    static saveAccessToken(accessToken: string) {
        localStorage.setItem('accessToken', accessToken);
    }

    static getAccessToken(): string | null {
        return localStorage.getItem('accessToken');
    }
}