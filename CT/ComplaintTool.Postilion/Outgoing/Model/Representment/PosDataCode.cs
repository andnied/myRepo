using System;

namespace ComplaintTool.Postilion.Outgoing.Model.Representment
{
    public class PosDataCode
    {
        //Position 1: post_tran_cust.pos_card_data_input_ability
        public string PosCardDataInputAbility { get; set; }
        //Position 2: post_tran_cust.pos_cardholder_auth_ability
        public string PosCardholderAuthAbility { get; set; }
        //Position 3: post_tran_cust.pos_card_capture_ability
        public string PosCardCaptureAbility { get; set; }
        //Position 4: post_tran_cust.pos_operating_environment
        public string PosOperatingEnvironment { get; set; }
        //Position 5: post_tran_cust.pos_cardholder_present
        public string PosCardholderPresent { get; set; }
        //Position 6: post_tran_cust.pos_card_present
        public string PosCardPresent { get; set; }
        //Position 7: post_tran_cust.pos_card_data_input_mode
        public string PosCardDataInputMode { get; set; }
        //Position 8: post_tran_cust.pos_cardholder_auth_method
        public string PosCardholderAuthMethod { get; set; }
        //Position 9: post_tran_cust.pos_cardholder_auth_entity
        public string PosCardholderAuthEntity { get; set; }
        //Position 10: post_tran_cust.pos_card_data_output_ability
        public string PosCardDataOutputAbility { get; set; }
        //Position 11: post_tran_cust.pos_terminal_output_ability
        public string PosTerminalOutputAbility { get; set; }
        //Position 12: post_tran_cust.pos_pin_capture_ability
        public string PosPinCaptureAbility { get; set; }
        //Position 13: post_tran_cust.pos_terminal_operator
        public string PosTerminalOperator { get; set; }
        //Position 14-15: post_tran_cust.pos_terminal_type
        public string PosTerminalType { get; set; }

        public override string ToString()
        {
            var response = !String.IsNullOrWhiteSpace(PosCardDataInputAbility) ? PosCardDataInputAbility : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosCardholderAuthAbility) ? PosCardholderAuthAbility : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosCardCaptureAbility) ? PosCardCaptureAbility : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosOperatingEnvironment) ? PosOperatingEnvironment : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosCardholderPresent) ? PosCardholderPresent : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosCardPresent) ? PosCardPresent : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosCardDataInputMode) ? PosCardDataInputMode : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosCardholderAuthMethod) ? PosCardholderAuthMethod : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosCardholderAuthEntity) ? PosCardholderAuthEntity : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosCardDataOutputAbility) ? PosCardDataOutputAbility : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosTerminalOutputAbility) ? PosTerminalOutputAbility : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosPinCaptureAbility) ? PosPinCaptureAbility : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosTerminalOperator) ? PosTerminalOperator : string.Empty;
            response += !String.IsNullOrWhiteSpace(PosTerminalType) ? PosTerminalType : string.Empty;
            return response;
        }
    }
}
