namespace _0_framework.Application.ZarinPal
{
    /// <summary>
    /// response
    ///
    /// {
    ///    "data": {
    ///        "wages": null,
    ///        "code": 100,
    ///        "message": "Paid",
    ///        "card_hash": "0866A6EAEA5CB085E4CF6EF19296BF19647552DD5F96F1E530DB3AE61837EFE7",
    ///        "card_pan": "999999******9999",
    ///        "ref_id": 76601,
    ///        "fee_type": "Merchant",
    ///        "fee": 1200,
    ///        "shaparak_fee": 1200,
    ///        "order_id": null
    ///    },
    ///    "errors": []
    ///}
    /// </summary>
    public class VerificationResponse
    {
        public Data data { get; set; }
        public List<string> errors { get; set; }

        public class Data
        {
            public int code { get; set; }
            public long ref_id { get; set; }
        }
    }
}