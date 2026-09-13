import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  CreatePurchaseBillRequest,
  PurchaseBillResponse,
} from '../../models/purchase-bill.model';

@Injectable({
  providedIn: 'root',
})
export class PurchaseBillService {
  private http = inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5035/api/purchase-bills';

  createPurchaseBill(
    request: CreatePurchaseBillRequest
  ): Observable<PurchaseBillResponse> {
    return this.http.post<PurchaseBillResponse>(
      this.apiUrl,
      request
    );
  }
}