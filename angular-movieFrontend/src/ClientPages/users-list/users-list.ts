import { Component, inject, OnInit } from '@angular/core';
import { UserInteractionService } from '../../Services/UserInteractionsService';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-users-list',
  imports: [RouterLink],
  templateUrl: './users-list.html',
  styleUrl: './users-list.css',
})
export class UsersList implements OnInit {
  userInteractionService = inject(UserInteractionService);
  items: any[] = []
  selectedType: string | null = null; // 'Planned' | 'InProgress' | 'Completed' | 'Ignored' | null
  selectedVote: string | null = null; // 'Liked' | 'Disliked' | null

  ngOnInit(): void {
    this.loadData();
    console.log(this.items);
  }

  onTypeChange(type: string | null): void {
    this.selectedType = type;
    this.loadData();
  }

  onVoteChange(vote: string | null): void {
    this.selectedVote = vote;
    this.loadData();
  }

  loadData(): void {
    this.userInteractionService.Get(this.selectedType, this.selectedVote)
      .subscribe(data => this.items = data);
  }
changeRoute(rawType:any, id:string): string {
    if(!rawType|| !id) {
      console.error('Item type or ID is undefined or null');
      return '';
    }
    const type = String(rawType).toLowerCase();
    if (type === 'movie') {
      console.log(`Navigating to movie with ID: ${id}`);
      return `/movie/${id}`;
    } else if (type === 'tvSeries') {
      return `/tvSeries/${id}`;
    } else if (type === 'game') {
      return `/game/${id}`;
    } else {
      return '';
    }
  }
}
