def numeral_sys_path(base)
    case base
    when "eEP"
        data.numerals.eepn.path
    when "lEP"
        data.numerals.lepn.path
    when "eU"
        data.numerals.eunin.path
    when "lU"
        data.numerals.lunin.path
    when "e"
        data.numerals.eun.path
    else
        data.numerals.mun.path
    end
end

def numeral_sys_alt(base)
    case base
    when "eEP"
        data.numerals.eepn.alt
    when "lEP"
        data.numerals.lepn.alt
    when "eU"
        data.numerals.eunin.alt
    when "lU"
        data.numerals.lunin.alt
    when "e"
        data.numerals.eun.alt
    else
        data.numerals.mun.alt
    end
end

def numeral_path(char)
    case char
    when "0"
        data.numerals.zero.path
    when "A"
        data.numerals.one.path
    when "B"
        data.numerals.two.path
    when "C"
        data.numerals.three.path
    when "D"
        data.numerals.four.path
    when "E"
        data.numerals.five.path
    when "F"
        data.numerals.six.path
    when "1"
        data.numerals.one_hex.path
    when "2"
        data.numerals.two_hex.path
    when "3"
        data.numerals.three_hex.path
    when "4"
        data.numerals.four_hex.path
    when "5"
        data.numerals.five_hex.path
    when "6"
        data.numerals.six_hex.path
    when "7"
        data.numerals.seven_hex.path
    when "8"
        data.numerals.eight_hex.path
    when "9"
        data.numerals.nine_hex.path
    else
        ""
    end
end

def numeral_alt(char)
    case char
    when "0"
        data.numerals.zero.alt
    when "A"
        data.numerals.one.alt
    when "B"
        data.numerals.two.alt
    when "C"
        data.numerals.three.alt
    when "D"
        data.numerals.four.alt
    when "E"
        data.numerals.five.alt
    when "F"
        data.numerals.six.alt
    when "1"
        data.numerals.one_hex.alt
    when "2"
        data.numerals.two_hex.alt
    when "3"
        data.numerals.three_hex.alt
    when "4"
        data.numerals.four_hex.alt
    when "5"
        data.numerals.five_hex.alt
    when "6"
        data.numerals.six_hex.alt
    when "7"
        data.numerals.seven_hex.alt
    when "8"
        data.numerals.eight_hex.alt
    when "9"
        data.numerals.nine_hex.alt
    else
        ""
    end
end
