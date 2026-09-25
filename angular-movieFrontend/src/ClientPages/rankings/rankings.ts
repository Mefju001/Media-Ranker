import { Component, inject, signal } from '@angular/core';
import { RankingsService } from '../../Services/RankingsService';
import { RouterLink } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { RankingsResponse } from '../../Data/Response/RankingsResponse';

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
  selectedType = signal<'movie' | 'tv' | 'game'|'all'>('all');
  items=signal<RankingsResponse[]>([]);

  onTypeChange(type: 'movie' | 'tv' | 'game' | 'all'): void {
    this.selectedType.set(type);
    this.getItems();
  }
  getItems(): void {
    this.rankingsService.getRankings({ type: this.selectedType() }).subscribe
    (data => {
      this.items.set(data);
    });
  }
  changeRoute(type:string, id:string): string {
    switch (type) {
    case 'movie':
      return `/movie/${id}`;
    case 'tv':
      return `/tvSeries/${id}`;
    case 'game':
      return `/game/${id}`;
    default:
      return '';
    }
  }
}
