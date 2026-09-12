export interface PurchaseBillItemRequest {
  item: string;
  locationCode: string;
  locationName: string;
  standardCost: number;
  standardPrice: number;
  quantity: number;
  discount: number;
}

export interface PurchaseBillItemResponse {
  id: number;
  item: string;
  locationCode: string;
  locationName: string;
  standardCost: number;
  standardPrice: number;
  quantity: number;
  discount: number;
  totalCost: number;
  totalSelling: number;
}