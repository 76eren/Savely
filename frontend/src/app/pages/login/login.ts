import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthField } from '../../shared/ui/auth-field/auth-field';
import { AuthShell } from '../../shared/ui/auth-shell/auth-shell';

@Component({
  selector: 'app-login',
  imports: [AuthField, AuthShell, RouterLink],
  templateUrl: './login.html'
})
export class Login {
  username = "";
  password = "";

  signIn(): void {
    console.log(this.username);
    console.log(this.password);
  }

}
