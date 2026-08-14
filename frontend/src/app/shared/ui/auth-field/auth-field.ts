import {Component, Input, model, ModelSignal} from '@angular/core';

@Component({
  selector: 'app-auth-field',
  imports: [],
  templateUrl: './auth-field.html'
})
export class AuthField {
  value: ModelSignal<string> = model('');

  @Input({ required: true }) id = '';
  @Input({ required: true }) label = '';
  @Input({ required: true }) name = '';
  @Input() type: 'email' | 'password' | 'text' = 'text';
  @Input() autocomplete = '';
  @Input() placeholder = '';
  @Input() minLength: number | null = null;
  @Input() showForgotPassword = false;

  passwordVisible = false;

  get inputType(): 'email' | 'password' | 'text' {
    return this.type === 'password' && this.passwordVisible ? 'text' : this.type;
  }
}
