import { Component, inject, OnInit } from '@angular/core';
import { UserInteractionService } from '../../Services/UserInteractionsService';

@Component({
  selector: 'app-users-list',
  imports: [],
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
}
