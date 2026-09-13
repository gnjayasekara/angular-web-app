export interface CreatePurchaseBillItemRequest {
  itemName: string;
  locationCode: string;
  standardCost: number;
  standardPrice: number;
  quantity: number;
  discountPercentage: number;
}

export interface CreatePurchaseBillRequest {
  items: CreatePurchaseBillItemRequest[];
}

export interface PurchaseBillItemResponse {
  id: number;
  itemName: string;
  locationCode: string;
  batchName: string;
  standardCost: number;
  standardPrice: number;
  quantity: number;
  discountPercentage: number;
  totalCost: number;
  totalSelling: number;
}

export interface PurchaseBillResponse {
  id: number;
  createdAt: string;
  totalItems: number;
  totalQuantity: number;
  items: PurchaseBillItemResponse[];
}