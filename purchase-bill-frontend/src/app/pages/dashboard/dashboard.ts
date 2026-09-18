import { DecimalPipe } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { DashboardService } from '../../core/services/dashboard.service';
import { DashboardResponse } from '../../models/dashboard.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [DecimalPipe, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class DashboardComponent implements OnInit {
  private readonly dashboardService = inject(DashboardService);

  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  readonly dashboard = signal<DashboardResponse>({
    latestOrders: [],
    oldestOrderItems: [],
    itemQuantities: [],
  });

  readonly totalQuantity = computed(() =>
    this.dashboard().itemQuantities.reduce(
      (total, item) => total + item.quantity,
      0,
    ),
  );

  readonly chartItems = computed(() => {
    const items = this.dashboard().itemQuantities;
    const total = this.totalQuantity();

    return items.map((item, index) => ({
      ...item,
      color: `hsl(${(index * 137.508) % 360}, 65%, 45%)`,
      percentage: total > 0 ? (item.quantity / total) * 100 : 0,
    }));
  });

  readonly donutBackground = computed(() => {
    if (this.totalQuantity() <= 0) {
      return '#e2e8f0';
    }

    let position = 0;

    const segments = this.chartItems().map((item) => {
      const start = position;
      position += item.percentage;

      return `${item.color} ${start}% ${position}%`;
    });

    return `conic-gradient(${segments.join(', ')})`;
  });

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    if (this.isLoading()) {
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.dashboardService.getDashboard().subscribe({
      next: (response) => {
        this.dashboard.set(response);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set(
          'Unable to load the dashboard. Please try again.',
        );
        this.isLoading.set(false);
      },
    });
  }
}