import { Component, inject } from '@angular/core';
import { PremieresService } from '../../Services/PremieresService';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-premieres',
  imports: [RouterLink],
  templateUrl: './premieres.html',
  styleUrl: './premieres.css',
})
export class Premieres {
  private readonly premieresService = inject(PremieresService);
  selectedScope: string = 'recent';
  selectedTypes: { value: 'movie' | 'tv' | 'game' | 'all'}[] = [
    { value: 'movie' },
    { value: 'tv' },
    { value: 'game' },
    { value: 'all' }
  ];
  selectedType: 'movie' | 'tv' | 'game'|'all' = 'all';
  items: any[] = [];

  onScopeChange(scope: string): void {
    this.selectedScope = scope;
    this.getItems();
  }
  onTypeChange(type: 'movie' | 'tv' | 'game' | 'all'): void {
    this.selectedType = type;
    this.getItems();
  }
  getItems(): void {
    this.premieresService.getPremieres({ scope: this.selectedScope, type: this.selectedType }).subscribe
    (data => {
      this.items = data;
    });
  }
  changeRoute(type:string, id:string): string {
    if (type === 'movie') {
      return `/movie/${id}`;
    } else if (type === 'tv') {
      return `/tvSeries/${id}`;
    } else if (type === 'game') {
      return `/game/${id}`;
    } else {
      return '';
    }
  }
}
