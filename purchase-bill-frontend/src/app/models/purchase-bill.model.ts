import {
  PurchaseBillItemRequest,
  PurchaseBillItemResponse,
} from './purchase-bill-item.model';

export interface PurchaseBillRequest {
  items: PurchaseBillItemRequest[];
}

export interface PurchaseBillResponse {
  id: number;
  totalItems: number;
  totalQuantity: number;
  items: PurchaseBillItemResponse[];
  createdAt: string;
}