import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AUTH_WALLPAPERS } from './auth-wallpapers';

@Component({
  selector: 'app-auth-shell',
  imports: [RouterLink],
  templateUrl: './auth-shell.html'
})
export class AuthShell {
  @Input({ required: true }) titleId = '';

  readonly wallpaper = this.pickWallpaper();

  private pickWallpaper(): string {
    if (AUTH_WALLPAPERS.length === 0) {
      return '';
    }

    const index = Math.floor(Math.random() * AUTH_WALLPAPERS.length);
    return `/login/${AUTH_WALLPAPERS[index]}`;
  }
}
