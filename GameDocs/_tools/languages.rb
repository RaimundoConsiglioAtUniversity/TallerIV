def change_language(locale)
  if locale == :en
    current_page.url.gsub('/es/', '/') # Remove Spanish prefix if exists
  else
    "/#{locale}" + current_page.url.gsub('/es/', '/')
  end
end

def localize_link(path, locale = I18n.locale) 
    if locale == :en
        path
    else
        "/#{locale}#{path}"
    end
end

def translate_if_translatable(key)
  I18n.exists?(key) ? t(key) : key
end
