import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { UserForLogin } from "../../models/user/userForLogin";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  loginUser(userForLogin: UserForLogin) {
    return new Promise<string>((resolve, reject) => {
      this.http.post('/api/account/login', userForLogin)
        .subscribe((data: any) => {
          resolve(data.accessTokenPayload);
        }, error => {
          reject(error);
        });
    });
  }
}
