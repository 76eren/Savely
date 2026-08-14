import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthField } from '../../shared/ui/auth-field/auth-field';
import { AuthShell } from '../../shared/ui/auth-shell/auth-shell';

@Component({
  selector: 'app-register',
  imports: [AuthField, AuthShell, RouterLink],
  templateUrl: './register.html',
})
export class Register {
  username = '';
  password = '';
  displayName = '';
  email = '';

  signUp(): void {
    console.log(this.username);
    console.log(this.password);
    console.log(this.displayName);
    console.log(this.email);
  }
}
