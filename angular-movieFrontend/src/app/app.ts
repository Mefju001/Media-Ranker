import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Footer } from '../footer/footer';
import { MovieWeb } from '../Pages/movie-web/movie-web';

@Component({
  selector: 'app-root',
  imports: [Footer,MovieWeb],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('angular-movieFrontend');
}
