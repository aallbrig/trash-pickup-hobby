const assert = require("assert");
const puppeteer = require("puppeteer");
const HEADLESS_MODE = process.env.HEADLESS_MODE || false;
const LANDING_PAGE = process.env.LANDING_PAGE || "http://localhost:8000";

let browser;
let page;

describe("The landing page for the trash pickup hobby website", () => {
  before(async () => {
    browser = await puppeteer.launch({
      headless: HEADLESS_MODE,
      args: ['--disable-dev-shm-usage']
    });
    page = await browser.newPage();
  });
  after(async () => {
    await browser.close();
  });

  it("Should exist", async () => {
    await page.goto(LANDING_PAGE);
    let title = await page.title();
    assert.equal(title, "Trash Pickup Hobby");
  });
});