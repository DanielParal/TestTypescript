import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  tokenKey = 'token';

  constructor(private http: HttpClient) { }

  setToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken() {
    const token = localStorage.getItem(this.tokenKey);
    if (token != null) {
      return JSON.parse(token);
    }
    return token;
  }

  isLoggedIn(): boolean {
    const token = this.getToken();

    if (token == null) {
      return false;
    }

    if (Date.now() <= token.exp * 1000) {
      return true;
    }

    localStorage.removeItem(this.tokenKey);
    return false;
  }

  getXSRFToken() {
    this.http.get('/api/token/xsrftoken')
      .subscribe(() => {
      }, error => {
        console.log(error);
      });
  }
}
