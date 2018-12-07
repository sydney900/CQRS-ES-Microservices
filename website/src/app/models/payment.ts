export class Payment {
  constructor(
    public bSB: string,
    public accountNumber: string,
    public accountName: string,
    public reference: string,
    public paymentAmount: number
  ) {}
}
