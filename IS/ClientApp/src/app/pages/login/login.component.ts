import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from "../../services/authorization/auth.service";
import { TokenService } from "../../services/authorization/token.service";
import { UserForLogin } from "../../models/user/userForLogin";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  formLogin: FormGroup;

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private tokenService: TokenService,
    private router: Router) { }

  ngOnInit() {
    this.tokenService.getXSRFToken();
    this.createLoginForm();
  }

  createLoginForm() {
    this.formLogin = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  async onLogin() {
    if (!this.formLogin.valid) {
      // show message - invalid login
      console.log("Involid inputs");
      return;
    }

    const userForLogin: UserForLogin = {
      username: this.formLogin.get('username').value,
      password: this.formLogin.get('password').value,
    };

    try {
      const accessPayload = await this.authService.loginUser(userForLogin);
      this.tokenService.setToken(accessPayload);
      this.router.navigate(['/']);
    } catch (e) {
      console.log(e);
    }
  }
}
