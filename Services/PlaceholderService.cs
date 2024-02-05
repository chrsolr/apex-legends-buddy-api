// import axios from 'axios'
// import cheerio from 'cheerio'
// import { colors } from '../utils/colors'
// import { cleanImageUrl } from '../utils/helpers'
// import {
//   LegendHeirloom,
//   LegendProfile,
//   LegendProfileAbility,
//   LegendProfileFinisher,
//   LegendProfileInfo,
//   LegendProfileLoadingScreen,
//   LegendProfileLoadingScreenDetails,
//   LegendProfileSkin,
//   LegendProfileSkydiveEmote,
//   Legends,
//   LegendsInsight,
// } from './legend.models'
//
// export class LegendsService {
//   private baseUrl = 'https://apexlegends.gamepedia.com'
//
//   public async getLegends(): Promise<Legends[]> {
//     const URL = `${this.baseUrl}/api.php?action=parse&format=json&page=Legends&redirects=1`
//     const { data } = await axios.get(URL)
//     const body = data.parse.text['*']
//     const $ = cheerio.load(body)
//     const legends = $('ul.gallery.mw-gallery-nolines li.gallerybox')
//       .map((index, element) => ({
//         id: index,
//         name: $(element).find('.gallerytext > p > a').text().trim(),
//         desc: $(element).find('.gallerytext > p > small').text().trim(),
//         imageUrl: cleanImageUrl(
//           $(element).find('.thumb > div > a > img').attr('data-src') ||
//             'https://via.placeholder.com/150',
//         ),
//         profileUrl: `${this.baseUrl}${$(element)
//           .find('.thumb > div > a')
//           .attr('href')}`,
//         classIconUrl: $(element)
//           .parent()
//           .prev()
//           .prev()
//           .find('a > img')
//           .attr('src'),
//         classDesc: $(element)
//           .parent()
//           .prev()
//           .text()
//           .replace(/[\r\n]/g, '')
//           .trim(),
//         className: $(element)
//           .parent()
//           .prev()
//           .prev()
//           .prev()
//           .find('span.mw-headline')
//           .text(),
//       }))
//       .get()
//
//     const insights = await this.getUsageRates()
//     return legends.map((value) => ({
//       ...value,
//       insight: insights.find((element) => element.name === value.name) || {
//         kpm: 'N/A',
//         name: value.name,
//         usageRate: 0,
//       },
//     }))
//   }
//
//   public async getLegendProfile(legendName: string): Promise<LegendProfile> {
//     const skins = await this.getSkins(legendName)
//     const bio = await this.getBio(legendName)
//     const quote = await this.getQuote(legendName)
//     const loadingScreens = await this.getLoadingScreens(legendName)
//     const finishers = await this.getFinishers(legendName)
//     const heirloom = await this.getHeirloom(legendName)
//     const skydiveEmotes = await this.getSkydiveEmotes(legendName)
//
//     const $ = await this.loadProfileHTML(legendName)
//
//     const info = LegendsService.parseInfobox($)
//     const abilities = this.parseAbilities($)
//
//     return {
//       bio,
//       skins,
//       quote,
//       abilities,
//       info,
//       loadingScreens,
//       finishers,
//       heirloom,
//       skydiveEmotes,
//     }
//   }
//
//   public async getSkins(legendName: string): Promise<LegendProfileSkin[]> {
//     const sectionIndex = await this.getSectionIndex(legendName, 'Skins')
//     const url = `${this.baseUrl}/api.php?action=parse&page=${legendName}&format=json&prop=text&section=${sectionIndex}`
//     const $ = cheerio.load((await axios.get(url)).data.parse.text['*'])
//     const $root = $('.tabbertab')
//
//     return $root
//       .map((index, element) => {
//         const $element = $(element)
//         const [rarity] = $element.attr('title')?.trim().split(' ') || ['Base']
//         // @ts-ignore
//         const color = colors.rarities[rarity]
//
//         const skins = $element
//           .find('.gallerybox')
//           .map((index, element) => {
//             const $title = $(element).find('.gallerytext span:eq(0)')
//             const $skinImage = $(element).find('.thumb img')
//             const $cost = $(element).find('.gallerytext span:eq(1)')
//             const costContent = $cost
//               .contents()
//               .map((i, v) => $(v).text().trim())
//               .get()
//               .filter((v) => v)
//
//             const name = $title.text().trim()
//             const rarity = $title.css('color').trim()
//             const imageUrl = $skinImage.attr('src')
//             const materialImageUrl = $cost.find('a img').attr('src')
//
//             const [materialCost] = costContent
//             const requirement = costContent.filter(
//               (v) => v && v.indexOf('[note') === -1,
//             )
//             return {
//               id: index,
//               name,
//               rarity,
//               imageUrl,
//               materialImageUrl,
//               materialCost,
//               requirement:
//                 requirement.length && requirement.length > 2
//                   ? requirement.splice(1, 3).join(' ')
//                   : undefined,
//             }
//           })
//           .get()
//         return { rarity, color, skins }
//       })
//       .get()
//   }
//
//   public async getBio(legendName: string): Promise<string[]> {
//     const sectionIndex = await this.getSectionIndex(legendName, 'Biography')
//     return cheerio
//       .load(
//         (
//           await axios.get(
//             `${this.baseUrl}/api.php?action=parse&page=${legendName}&format=json&prop=text&section=${sectionIndex}`,
//           )
//         ).data.parse.text['*'],
//       )('p')
//       .text()
//       .trim()
//       .replace(/\[[0-9]]/g, '')
//       .split('\n')
//       .filter((value) => value)
//   }
//
//   public async getQuote(legendName: string): Promise<string> {
//     return cheerio
//       .load(
//         (
//           await axios.get(
//             `${this.baseUrl}/api.php?action=parse&page=${legendName}&format=json&prop=text&section=0`,
//           )
//         ).data.parse.text['*'],
//       )('table tr:first-child td:nth-child(2)')
//       .text()
//       .trim()
//       .replace(/\[[0-9]]/g, '')
//       .replace(/[\r\n]/g, '')
//   }
//
//   public async getUsageRates(): Promise<LegendsInsight[]> {
//     const URL = 'https://tracker.gg/apex/insights'
//     const { data } = await axios.get(URL)
//     const $ = cheerio.load(data)
//     const usage = $('.usage__content .insight-bar')
//       .map((_, element) => ({
//         name: $(element).find('.insight-bar__label').text().trim(),
//         usageRate: Number(
//           (
//             +$(element)
//               .find('.insight-bar__value')
//               .text()
//               .trim()
//               .replace('%', '') / 100
//           ).toFixed(3),
//         ),
//       }))
//       .get()
//
//     const kpm = $('table tbody tr')
//       .map((_, element) => ({
//         name: $(element).find('td:nth-child(1)').text().trim(),
//         kpm: $(element).find('td:nth-child(2)').text().trim()
//           ? $(element).find('td:nth-child(2)').text().trim()
//           : 'N/A',
//       }))
//       .get()
//
//     return usage.map((element) => ({
//       ...element,
//       kpm: kpm.find((value) => value.name === element.name).kpm,
//     }))
//   }
//
//   public async getLoadingScreens(
//     legendName: string,
//   ): Promise<LegendProfileLoadingScreen[]> {
//     const sectionIndex = await this.getSectionIndex(
//       legendName,
//       'Loading_Screens',
//     )
//     const url = `${this.baseUrl}/api.php?action=parse&page=${legendName}&format=json&prop=text&section=${sectionIndex}`
//     const $ = cheerio.load((await axios.get(url)).data.parse.text['*'])
//     return $('.gallerybox')
//       .map((index, element) => {
//         const $title = $(element).find('.gallerytext span:eq(0)')
//         const $imageUrl = $(element).find('.thumb img')
//
//         const name = $title.text().trim()
//         const rarity = $title.css('color').trim()
//         const imageUrl = $imageUrl.attr('src')
//
//         return {
//           id: index,
//           name,
//           rarity,
//           imageUrl,
//         }
//       })
//       .get()
//   }
//
//   public async getLoadingScreenDetails(
//     title: string,
//   ): Promise<LegendProfileLoadingScreenDetails> {
//     const $ = cheerio.load(
//       (
//         await axios.get(
//           `${this.baseUrl}/api.php?action=parse&format=json&page=Loading_Screen`,
//         )
//       ).data.parse.text['*'],
//     )
//
//     const $rootElement = $(`tr:contains(${title})`)
//     const legendName = $($rootElement).find('a:nth-child(3)').text().trim()
//     const descriptions = $($rootElement)
//       .eq(0)
//       .text()
//       .trim()
//       .replace(`${title} - ${legendName}`, '')
//       .replace(title, '')
//       .split('\n')
//       .filter((value) => value)
//
//     const earnedOn = descriptions.pop()?.trim()
//     return { title, earnedOn, descriptions }
//   }
//
//   public async getFinishers(
//     legendName: string,
//   ): Promise<LegendProfileFinisher[]> {
//     const sectionIndex = await this.getSectionIndex(legendName, 'Finishers')
//     const url = `${this.baseUrl}/api.php?action=parse&page=${legendName}&format=json&prop=text&section=${sectionIndex}`
//     const $ = cheerio.load((await axios.get(url)).data.parse.text['*'])
//
//     const result = $('.gallery.mw-gallery-nolines li')
//       .map((index, element) => {
//         const src = $(element).find('video').attr('src')
//         const textContent = $(element)
//           .find('.gallerytext')
//           .contents()
//           .map((i, v) => $(v).text().trim())
//           .get()
//           .filter((v) => v)[0]
//           .trim()
//           .replace('Default', '')
//           .split(' ')
//
//         if (index > 0) {
//           textContent.pop()
//         }
//
//         const name = textContent.join(' ')
//         const rarity = $(element)
//           .find('.gallerytext p span')
//           .css('color')
//           .trim()
//
//         return {
//           name,
//           rarity,
//           videoUrl: src,
//           materialImageUrl: '',
//           materialCost: '',
//         }
//       })
//       .get()
//     return result as LegendProfileFinisher[]
//   }
//
//   public async getHeirloom(
//     legendName: string,
//   ): Promise<LegendHeirloom | undefined> {
//     const sectionIndex = await this.getSectionIndex(legendName, 'Heirloom_Set')
//
//     if (!sectionIndex) {
//       return undefined
//     }
//
//     const url = `${this.baseUrl}/api.php?action=parse&page=${legendName}&format=json&prop=text&section=${sectionIndex}`
//     const $ = cheerio.load((await axios.get(url)).data.parse.text['*'])
//     const imageUrl = $('img').attr('src')
//     const desc = $('ul li')
//       .map((index, element) =>
//         $(element)
//           .text()
//           .trim()
//           .split(':')
//           .map((v) => v.trim()),
//       )
//       .get()
//       .reduce(
//         (memo, current, index) => {
//           if (index === 1) {
//             memo.name = current
//           }
//
//           if (index === 3) {
//             memo.banner = current
//           }
//
//           if (index === 5) {
//             memo.quip = current
//           }
//           return memo
//         },
//         {
//           rarity: colors.rarities.Heirloom,
//           name: null,
//           banner: null,
//           quip: null,
//         },
//       )
//
//     return { imageUrl, desc }
//   }
//
//   public async getSkydiveEmotes(
//     legendName: string,
//   ): Promise<LegendProfileSkydiveEmote[]> {
//     const sectionIndex = await this.getSectionIndex(
//       legendName,
//       'Skydive_Emotes',
//     )
//
//     const url = `${this.baseUrl}/api.php?action=parse&page=${legendName}&format=json&prop=text&section=${sectionIndex}`
//     const $ = cheerio.load((await axios.get(url)).data.parse.text['*'])
//
//     const result = $('.gallery.mw-gallery-nolines li')
//       .map((index, element) => {
//         const src = $(element).find('video').attr('src')
//         const textContent = $(element)
//           .find('.gallerytext')
//           .contents()
//           .map((i, v) => $(v).text().trim())
//           .get()
//           .filter((v) => v)[0]
//           .trim()
//           .replace('Level', '')
//           .split(' ')
//
//         textContent.pop()
//
//         const name = textContent.join(' ')
//         const rarity = $(element)
//           .find('.gallerytext p span')
//           .css('color')
//           .trim()
//
//         return {
//           name,
//           rarity,
//           videoUrl: src,
//         }
//       })
//       .get()
//
//     return result as LegendProfileSkydiveEmote[]
//   }
//
//   private async getSectionIndex(
//     legendName: string,
//     sectionName: string,
//   ): Promise<number> {
//     const sections: any[] = (
//       await axios.get(
//         `${this.baseUrl}/api.php?action=parse&format=json&prop=sections&page=${legendName}`,
//       )
//     ).data.parse.sections
//     return sections.find((e) => e.anchor === sectionName)?.index
//   }
//
//   private async loadProfileHTML(legendName: string): Promise<cheerio.Root> {
//     return cheerio.load(
//       (
//         await axios.get(
//           `${this.baseUrl}/api.php?action=parse&format=json&page=${legendName}`,
//         )
//       ).data.parse.text['*'],
//     )
//   }
//
//   private parseAbilities($: cheerio.Root): LegendProfileAbility[] {
//     const $abilities = $('.ability-container')
//     return $($abilities)
//       .map((_, element) => ({
//         name: $(element).find('.ability-image').eq(0).siblings().text().trim(),
//         imageUrl: $(element).find('.ability-image > a > img').eq(0).attr('src'),
//         description: $(element)
//           .find('.ability-header')
//           .map((i, e) => ({
//             name: $(e)
//               .text()
//               .replace(/[\r\n]/g, ''),
//             value: $(e).siblings().text().trim(),
//           }))
//           .get(),
//         info: $(element)
//           .find('.tabber-ability [title="Info"] li')
//           .map((i, e) => $(e).text().trim())
//           .get(),
//         interactions: $(element)
//           .find('.tabber-ability [title="Interactions"] li')
//           .map((i, e) => $(e).text().trim())
//           .get(),
//         tips: $(element)
//           .find('.tabber-ability [title="Tips"] li')
//           .map((i, e) => $(e).text().trim())
//           .get(),
//       }))
//       .get()
//   }
//
//   private static parseInfobox($: cheerio.Root): LegendProfileInfo {
//     const $infobox = $('.infobox.infobox tr')
//     return {
//       name: $($infobox)
//         .eq(0)
//         .find('th')
//         .text()
//         .trim()
//         .replace(/[\r\n]/g, ''),
//       imageUrl:
//         $($infobox).eq(1).find('td > a > img').attr('src') ||
//         'https://via.placeholder.com/150',
//       desc: $($infobox)
//         .eq(2)
//         .find('td > i')
//         .text()
//         .trim()
//         .replace(/[\r\n]/g, ''),
//       realName: $($infobox)
//         .find('.infobox-row-name:contains(Real Name)')
//         .siblings()
//         .text()
//         .trim(),
//       gender: $($infobox)
//         .find('.infobox-row-name:contains(Gender)')
//         .siblings()
//         .text()
//         .trim(),
//       age: $($infobox)
//         .find('.infobox-row-name:contains(Age)')
//         .siblings()
//         .text()
//         .trim(),
//       weight: $($infobox)
//         .find('.infobox-row-name:contains(Weight)')
//         .siblings()
//         .text()
//         .trim(),
//       height: $($infobox)
//         .find('.infobox-row-name:contains(Height)')
//         .siblings()
//         .text()
//         .trim(),
//       homeWorld: $($infobox)
//         .find('.infobox-row-name:contains(Home World)')
//         .siblings()
//         .text()
//         .trim(),
//       voiceActor: $($infobox)
//         .find('.infobox-row-name:contains(Voice Actor)')
//         .siblings()
//         .text()
//         .trim(),
//     }
//   }
// }
//
// export const legendsService = new LegendsService()
