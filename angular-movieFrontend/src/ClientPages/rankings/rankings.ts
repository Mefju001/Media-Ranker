import { Component, inject } from '@angular/core';
import { RankingsService } from '../../Services/RankingsService';
import { RouterLink } from '@angular/router';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-rankings',
  imports: [RouterLink, DecimalPipe],
  templateUrl: './rankings.html',
  styleUrl: './rankings.css',
})
export class Rankings {
  private readonly rankingsService = inject(RankingsService);
  selectedTypes: { value: 'movie' | 'tv' | 'game' | 'all'}[] = [
    { value: 'movie' },
    { value: 'tv' },
    { value: 'game' },
    { value: 'all' }
  ];
  selectedType: 'movie' | 'tv' | 'game'|'all' = 'all';
  items: any[] = [];

  onTypeChange(type: 'movie' | 'tv' | 'game' | 'all'): void {
    this.selectedType = type;
    this.getItems();
  }
  getItems(): void {
    this.rankingsService.getRankings({ type: this.selectedType }).subscribe
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
