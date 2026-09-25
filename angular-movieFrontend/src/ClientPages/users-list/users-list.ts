import { Component, inject, OnInit, signal } from '@angular/core';
import { UserInteractionService } from '../../Services/UserInteractionsService';
import { RouterLink } from '@angular/router';
import { UserInteractionsResponse } from '../../Data/Response/UserInteractionsResponse';

@Component({
  selector: 'app-users-list',
  imports: [RouterLink],
  templateUrl: './users-list.html',
  styleUrl: './users-list.css',
})
export class UsersList implements OnInit {
  userInteractionService = inject(UserInteractionService);
  items = signal<UserInteractionsResponse[]>([]);
  selectedType = signal<string | null>(null);
  selectedVote = signal<string | null>(null);

  ngOnInit(): void {
    this.loadData();
  }

  onTypeChange(type: string | null): void {
    this.selectedType.set(type);
    this.loadData();
  }

  onVoteChange(vote: string | null): void {
    this.selectedVote.set(vote);
    this.loadData();
  }

  loadData(): void {
    this.userInteractionService.Get(this.selectedType(), this.selectedVote())
      .subscribe(data => this.items.set(data));
  }
changeRoute(rawType:any, id:string): string {
    if(!rawType|| !id) {
      console.error('Item type or ID is undefined or null');
      return '';
    }
    const type = String(rawType).toLowerCase();
    switch (type) {
          case 'movie':
            return `/movie/${id}`;
          case 'tvseries':
            return `/tvSeries/${id}`;
          case 'game':
            return `/game/${id}`;
          default:
            return '';
        }
  }
}
