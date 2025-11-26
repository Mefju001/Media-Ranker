import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Footer } from '../Pages/footer/footer';
import { MovieWeb } from '../Pages/movie-web/movie-web';
import { Header } from '../Pages/header/header';

@Component({
  selector: 'app-root',
  imports: [Footer,MovieWeb,Header],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Media Ranker');
}
