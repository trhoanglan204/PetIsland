# 🐞 TODO: Add More Vulnerabilities for Pentesting

This list includes additional bugs that can be added to extend this branch:

## ✅ Already Implemented
- [x] SQL Injection (SQLi)
- [x] Cross-Site Scripting (XSS)

## 🧪 Recommended Additions
- [ ] Insecure File Upload (e.g., no extension check, upload `.html` or `.aspx`)
- [ ] Command Injection (e.g., user input passed to `Process.Start`)
- [ ] Insecure Direct Object Reference (IDOR)
- [ ] Broken Authentication (hardcoded password, weak session logic)
- [ ] Cross-Site Request Forgery (CSRF)
- [ ] Sensitive Data Exposure (e.g., display connection strings)
- [ ] Security Misconfiguration (e.g., verbose error pages)
- [ ] Unvalidated Redirects and Forwards
- [ ] Clickjacking (no `X-Frame-Options`)
- [ ] Reflected File Download

## Notes
- Each vulnerability should be **clearly documented** in code or comments.
- Ensure students can **trigger and understand** each issue easily.

> Want to build a full vulnerable lab? Gradually implement the OWASP Top 10.